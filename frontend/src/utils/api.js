import { ref } from 'vue';

const BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';

// Reactive tracking for request latency
const latencies = ref([]);
export const globalAvgLatency = ref(0);

const trackLatency = (start) => {
  const duration = Date.now() - start;
  latencies.value.push(duration);
  // Keep only the last 20 requests for average
  if (latencies.value.length > 20) latencies.value.shift();
  
  const sum = latencies.value.reduce((a, b) => a + b, 0);
  globalAvgLatency.value = Math.round(sum / latencies.value.length);
};

const getHeaders = () => {
  const token = localStorage.getItem('dcs_auth_token');
  const headers = {
    'Content-Type': 'application/json',
  };
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  return headers;
};

const handleResponse = async (response, startTime) => {
  trackLatency(startTime);
  
  const text = await response.text();
  let data;
  try {
    data = text ? JSON.parse(text) : null;
  } catch (e) {
    data = { message: text };
  }

  if (!response.ok) {
    console.error('API Error:', {
      status: response.status,
      statusText: response.statusText,
      url: response.url,
      body: data
    });

    throw new Error(data?.message || data?.title || `Error ${response.status}: ${response.statusText}`);
  }

  return data;
};

export const api = {
  get: async (endpoint) => {
    const startTime = Date.now();
    const response = await fetch(`${BASE_URL}${endpoint}`, {
      method: 'GET',
      headers: getHeaders(),
    });
    return handleResponse(response, startTime);
  },

  post: async (endpoint, data, isMultipart = false) => {
    const startTime = Date.now();
    const headers = getHeaders();
    if (isMultipart) {
      delete headers['Content-Type']; // Let browser set boundary
    }

    const response = await fetch(`${BASE_URL}${endpoint}`, {
      method: 'POST',
      headers: headers,
      body: isMultipart ? data : JSON.stringify(data),
    });
    return handleResponse(response, startTime);
  },

  setToken: (token) => {
    localStorage.setItem('dcs_auth_token', token);
  },

  clearToken: () => {
    localStorage.removeItem('dcs_auth_token');
  }
};
