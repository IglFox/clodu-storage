/**
 * API Configuration
 * Centralized configuration for all API endpoints, settings, and constants
 */

// API Base Configuration
export const API_CONFIG = {
  // Base URL for API requests
  baseURL: import.meta.env.VITE_API_URL || 'https://api.nebulacloud.com/v1',

  // Alternative endpoints (development, staging, production)
  endpoints: {
    development: 'http://localhost:3000/api/v1',
    staging: 'https://staging-api.nebulacloud.com/v1',
    production: 'https://api.nebulacloud.com/v1',
  },

  // Request timeout in milliseconds
  timeout: 30000,

  // Enable request logging in development
  debugMode: import.meta.env.DEV,

  // CORS settings
  cors: {
    enabled: true,
    credentials: 'include',
  },
}

// Authentication Endpoints
export const AUTH_ENDPOINTS = {
  register: '/auth/register',
  login: '/auth/login',
  logout: '/auth/logout',
  refresh: '/auth/refresh',
  verify: '/auth/verify',
  resetPassword: '/auth/reset-password',
  confirmEmail: '/auth/confirm-email',
}

// Files Management Endpoints
export const FILES_ENDPOINTS = {
  list: '/files',
  upload: '/files/upload',
  download: '/files/:id/download',
  delete: '/files/:id',
  move: '/files/:id/move',
  copy: '/files/:id/copy',
  search: '/files/search',
  getById: '/files/:id',
}

// Folders Endpoints
export const FOLDERS_ENDPOINTS = {
  list: '/folders',
  create: '/folders',
  delete: '/folders/:id',
  rename: '/folders/:id',
  getById: '/folders/:id',
}

// Analytics Endpoints
export const ANALYTICS_ENDPOINTS = {
  stats: '/analytics/stats',
  activity: '/analytics/activity',
  storageUsage: '/analytics/storage',
  fileTypeDistribution: '/analytics/file-types',
}

// User Endpoints
export const USER_ENDPOINTS = {
  profile: '/users/profile',
  updateProfile: '/users/profile',
  settings: '/users/settings',
  updateSettings: '/users/settings',
  changePassword: '/users/change-password',
  deleteAccount: '/users/account/delete',
}

// Storage Quota Constants
export const STORAGE_CONFIG = {
  // Maximum storage in bytes (10 GB)
  maxStorage: 10 * 1024 * 1024 * 1024,

  // Warning threshold (80% of max)
  warningThreshold: 0.8,

  // Critical threshold (95% of max)
  criticalThreshold: 0.95,

  // File upload limits
  maxFileSize: 5 * 1024 * 1024 * 1024, // 5 GB
  maxBatchUploadSize: 10 * 1024 * 1024 * 1024, // 10 GB

  // Allowed file types
  allowedFileTypes: [
    'application/pdf',
    'image/jpeg',
    'image/png',
    'image/gif',
    'text/plain',
    'text/markdown',
    'application/vnd.ms-excel',
    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    'application/msword',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    'application/zip',
    'video/mp4',
    'video/mpeg',
    'audio/mpeg',
  ],
}

// User Registration Settings
export const REGISTRATION_CONFIG = {
  // Minimum password length
  minPasswordLength: 8,

  // Password must contain at least one uppercase letter
  requireUppercase: true,

  // Password must contain at least one number
  requireNumbers: true,

  // Password must contain at least one special character
  requireSpecialChars: true,

  // Email verification required
  emailVerificationRequired: true,

  // Email verification timeout (in hours)
  emailVerificationTimeout: 24,

  // Username minimum length
  minUsernameLength: 3,

  // Username maximum length
  maxUsernameLength: 30,

  // Allowed username characters
  usernameRegex: /^[a-zA-Z0-9_-]+$/,

  // Default storage plan for new users (in GB)
  defaultStorageGB: 5,
}

// API Headers
export const DEFAULT_HEADERS = {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  'X-API-Version': 'v1',
  'X-Client-Name': 'nebula-cloud',
  'X-Client-Version': '1.0.0',
}

// Rate Limiting
export const RATE_LIMITS = {
  // Requests per minute
  requestsPerMinute: 60,

  // Uploads per hour
  uploadsPerHour: 100,

  // Downloads per hour
  downloadsPerHour: 200,

  // Search requests per minute
  searchPerMinute: 30,
}

// Retry Configuration
export const RETRY_CONFIG = {
  // Maximum number of retry attempts
  maxRetries: 3,

  // Delay between retries (in milliseconds)
  retryDelay: 1000,

  // Exponential backoff multiplier
  backoffMultiplier: 2,

  // Status codes that should trigger a retry
  retryStatusCodes: [408, 429, 500, 502, 503, 504],
}

// API Response Messages
export const API_MESSAGES = {
  // Success messages
  success: {
    registration: 'Регистрация успешна! Проверьте вашу электронную почту для подтверждения.',
    login: 'Успешный вход в систему!',
    fileUploaded: 'Файл успешно загружен.',
    fileDeleted: 'Файл успешно удален.',
    folderCreated: 'Папка успешно создана.',
  },

  // Error messages
  error: {
    invalidEmail: 'Пожалуйста, введите корректный адрес электронной почты.',
    weakPassword: 'Пароль не соответствует требованиям безопасности.',
    userExists: 'Пользователь с такой электронной почтой уже существует.',
    invalidCredentials: 'Неверный адрес электронной почты или пароль.',
    serverError: 'Ошибка сервера. Пожалуйста, попробуйте позже.',
    networkError: 'Ошибка сети. Проверьте ваше соединение.',
    fileTooBig: 'Размер файла превышает максимально допустимый.',
    storageQuotaExceeded: 'Превышена квота хранилища.',
    unauthorized: 'Необходимо войти в систему.',
  },
}

// Export function to get the appropriate API URL based on environment
export function getApiUrl(env?: 'development' | 'staging' | 'production'): string {
  const environment = env || import.meta.env.MODE
  return API_CONFIG.endpoints[environment as keyof typeof API_CONFIG.endpoints] || API_CONFIG.baseURL
}

// Export function to build full endpoint URL
export function buildEndpointUrl(endpoint: string, params?: Record<string, string>): string {
  let url = `${getApiUrl()}${endpoint}`

  if (params) {
    Object.entries(params).forEach(([key, value]) => {
      url = url.replace(`:${key}`, value)
    })
  }

  return url
}
