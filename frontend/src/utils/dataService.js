/**
 * DataService
 * Handles sending ingestion batches and securely backing up data to an external data microservice.
 */

export const DataService = {
  /**
   * Sends an ingested file metadata or parsed contents to a microservice
   * @param {string} baseUrl - Base URL of the data/ingest microservice
   * @param {Object} payload - Ingestion record details
   * @param {string} token - Authentication JWT token
   * @returns {Promise<any>} Response from the ingress microservice
   */
  async sendIngestRecord(baseUrl, payload, token) {
    if (!baseUrl) {
      throw new Error('Data microservice URL is not configured.');
    }

    const cleanUrl = baseUrl.endsWith('/') ? baseUrl.slice(0, -1) : baseUrl;
    const response = await fetch(`${cleanUrl}/api/ingest`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(payload)
    });

    if (!response.ok) {
      const errData = await response.json().catch(() => ({}));
      throw new Error(errData.message || errData.error || `Failed to submit to ingest microservice: ${response.status}`);
    }

    return await response.json().catch(() => ({}));
  },

  /**
   * Retrieves the stored URL for the data ingestion microservice
   * @returns {string}
   */
  getIngestApiUrl() {
    return localStorage.getItem('customDataApiUrl') || import.meta.env.VITE_DATA_API_URL || '';
  },

  /**
   * Persists the data ingestion microservice URL to localStorage
   * @param {string} url 
   */
  setIngestApiUrl(url) {
    if (url) {
      localStorage.setItem('customDataApiUrl', url.trim());
    } else {
      localStorage.removeItem('customDataApiUrl');
    }
  }
};
