// src/composables/useDashboard.js
import { ref, onMounted } from "vue";
import { httpGet, httpPost } from "@/composables/useHttp";

/* ----------  уже есть (files, storage, …) ---------- */

export function useDashboard() {
    const files = ref([]);
    const storage = ref({ used: 0, total: 0, percent: 0 });
    const security = ref([]);
    const config = ref([]);
    const loading = ref(false);
    const error = ref(null);

    /* ----------  загрузка всех данных ---------- */
    async function loadAll() {
        loading.value = true;
        error.value = null;
        try {
            const [filesRes, storageRes, secRes, cfgRes] = await Promise.all([
                httpGet("/files"),
                httpGet("/storage"),
                httpGet("/security"),
                httpGet("/config"),
            ]);
            files.value = filesRes;
            storage.value = storageRes;
            security.value = secRes;
            config.value = cfgRes;
        } catch (e) {
            error.value = e.message;
        } finally {
            loading.value = false;
        }
    }

    /* ----------  ЗАГРУЗКА ФАЙЛА ---------- */
    async function uploadFile(file) {
        const form = new FormData();
        form.append("file", file);

        try {
            loading.value = true;
            const uploaded = await httpPost("/files/upload", form, {
                // fetch не ставит Content‑Type для FormData автоматически
                // поэтому просто передаём FormData без заголовков
            });
            // После успешной загрузки сразу добавляем в список
            files.value.push(uploaded);
        } catch (e) {
            error.value = `Не удалось загрузить файл: ${e.message}`;
        } finally {
            loading.value = false;
        }
    }

    /* ----------  СКАЧИВАНИЕ ФАЙЛА ---------- */
    async function downloadFile(fileId, fileName = "download") {
        try {
            loading.value = true;
            const blob = await httpGet(`/files/${fileId}/download`, {
                // fetch возвращает Response, нам нужен blob
                responseType: "blob", // наш httpGet будет проверять это поле
            });

            // Создаём объект URL и «кликаем» по нему
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = fileName; // имя, которое хочет пользователь
            a.click();
            window.URL.revokeObjectURL(url);
        } catch (e) {
            error.value = `Не удалось скачать файл: ${e.message}`;
        } finally {
            loading.value = false;
        }
    }

    /* ----------  init ---------- */
    onMounted(loadAll);

    return {
        files,
        storage,
        security,
        config,
        loading,
        error,
        refetch: loadAll,
        uploadFile,
        downloadFile,
    };
}
