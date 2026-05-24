<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import { CryptoService } from '../utils/crypto';
import { ParserService } from '../utils/parser';

const router = useRouter();
const isProcessing = ref(false);
const status = ref('');
const results = ref([]);

onMounted(() => {
  const savedResults = localStorage.getItem('vaulted_results');
  if (savedResults) {
    results.value = JSON.parse(savedResults);
  }
});

watch(results, (newVal) => {
  localStorage.setItem('vaulted_results', JSON.stringify(newVal));
}, { deep: true });

const mode = ref('SAFE'); // 'CRYPT-MAX' or 'SAFE'

const handleFile = async (e) => {
  const files = Array.from(e.target.files);
  if (files.length === 0) return;

  if (mode.value === 'CRYPT-MAX' && !localStorage.getItem('masterKey')) {
    alert('Master Key is required for CRYPT-MAX mode. Please set it up in your Profile.');
    router.push('/profile');
    return;
  }

  isProcessing.value = true;
  const newResults = [];
  
  try {
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      status.value = `Processing [${i + 1}/${files.length}]: ${file.name}...`;

      if (mode.value === 'CRYPT-MAX') {
        const arrayBuffer = await file.arrayBuffer();
        const key = await CryptoService.generateKey();
        const encrypted = await CryptoService.encrypt(new Uint8Array(arrayBuffer), key);
        
        const keyJWK = await CryptoService.exportKey(key);
        const keyId = `key_${Date.now()}_${i}`;
        await CryptoService.saveKey(keyId, keyJWK);

        newResults.push({
          mode: 'CRYPT-MAX (Zero-Knowledge)',
          filename: file.name,
          keyId: keyId,
          encryptedSize: encrypted.content.byteLength,
          details: 'File binary encrypted on client.'
        });
      } else {
        const text = await ParserService.parseFile(file);
        
        if (text) {
          newResults.push({
            mode: 'SAFE (Server-decrypt)',
            filename: file.name,
            textLength: text.length,
            preview: text.substring(0, 200) + (text.length > 200 ? '...' : ''),
            details: 'Text extracted.'
          });
        } else {
          newResults.push({
            mode: 'SAFE (Server-decrypt)',
            filename: file.name,
            details: 'Raw file uploaded for analysis.'
          });
        }
      }
    }
    results.value = [...results.value, ...newResults];
    status.value = `Success! ${files.length} files processed.`;
  } catch (err) {
    status.value = `Error finishing batch: ${err.message}`;
  } finally {
    isProcessing.value = false;
  }
};
</script>

<template>
  <div>
    <nav>
      <strong>CloudVault</strong>
      | <button @click="$router.push('/profile')">Profile</button>
    </nav>
    <hr />

    <main>
      <section>
        <h2>Secure Data Ingest</h2>
        
        <div>
          <label>
            <input type="radio" value="CRYPT-MAX" v-model="mode" />
            <strong>Режим «CRYPT-MAX»</strong> (Zero-Knowledge)
          </label>
          <label>
            <input type="radio" value="SAFE" v-model="mode" />
            <strong>Режим «SAFE»</strong> (Server-decrypt)
          </label>
        </div>

        <div>
          <input type="file" id="file" @change="handleFile" multiple hidden />
          <label for="file">
            <div><strong>Click to Begin Ingestion</strong></div>
            <small>Select one or more files (PDF, DOCX, TXT)</small>
          </label>
        </div>

        <div v-if="status">
          <span v-if="isProcessing">⏳ </span>
          <span>{{ status }}</span>
        </div>

        <div v-if="results.length > 0">
          <h3>Vault Processing Results</h3>
          <div v-for="(res, idx) in results" :key="idx">
            <div>
              <strong>{{ res.filename }}</strong>
              <small>{{ res.mode }}</small>
            </div>
            <p>{{ res.details }}</p>
            <ul>
              <li v-if="res.keyId">Key ID: <code>{{ res.keyId }}</code></li>
              <li v-if="res.encryptedSize">Vault Size: {{ res.encryptedSize }} bytes</li>
              <li v-if="res.textLength">Extracted Chars: {{ res.textLength }}</li>
            </ul>
            <div v-if="res.preview">
              <pre>{{ res.preview }}</pre>
            </div>
          </div>
        </div>
      </section>

      <br />
      <hr />
    </main>
  </div>
</template>
