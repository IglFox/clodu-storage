import localforage from 'localforage';

localforage.config({
  name: 'CloudVault',
  storeName: 'keys'
});

export const CryptoService = {
  /**
   * Generates a new AES-GCM key for encryption
   */
  async generateKey() {
    return await window.crypto.subtle.generateKey(
      {
        name: 'AES-GCM',
        length: 256
      },
      true,
      ['encrypt', 'decrypt']
    );
  },

  /**
   * Exports a key to a serializable format (JWK)
   */
  async exportKey(key) {
    return await window.crypto.subtle.exportKey('jwk', key);
  },

  /**
   * Imports a key from JWK format
   */
  async importKey(jwk) {
    return await window.crypto.subtle.importKey(
      'jwk',
      jwk,
      { name: 'AES-GCM' },
      true,
      ['encrypt', 'decrypt']
    );
  },

  /**
   * Encrypts a file or string
   */
  async encrypt(data, key) {
    const encoder = new TextEncoder();
    const encodedData = typeof data === 'string' ? encoder.encode(data) : data;
    const iv = window.crypto.getRandomValues(new Uint8Array(12));
    
    const encryptedContent = await window.crypto.subtle.encrypt(
      {
        name: 'AES-GCM',
        iv: iv
      },
      key,
      encodedData
    );

    return {
      content: encryptedContent,
      iv: iv
    };
  },

  /**
   * Decrypts content
   */
  async decrypt(encryptedData, key, iv) {
    const decryptedContent = await window.crypto.subtle.decrypt(
      {
        name: 'AES-GCM',
        iv: iv
      },
      key,
      encryptedData
    );

    return decryptedContent;
  },

  /**
   * Store a key in local storage
   */
  async saveKey(id, keyJWK) {
    await localforage.setItem(id, keyJWK);
  },

  /**
   * Retrieve a key from local storage
   */
  async getKey(id) {
    return await localforage.getItem(id);
  }
};
