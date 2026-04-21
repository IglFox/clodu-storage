# Nebula Cloud - API Setup Guide

## Overview

This document explains how to configure and integrate the Nebula Cloud application with your backend API.

## API Configuration File

The main API configuration is located in `src/config/apiConfig.ts`. This file contains all API endpoints, settings, and constants.

### Configuration Structure

```typescript
// Base API configuration
export const API_CONFIG = {
  baseURL: string          // Base URL for API requests
  endpoints: {...}         // Environment-specific endpoints
  timeout: number          // Request timeout in milliseconds
  debugMode: boolean       // Enable request logging
  cors: {...}             // CORS settings
}

// Authentication endpoints
export const AUTH_ENDPOINTS = {
  register: '/auth/register'
  login: '/auth/login'
  logout: '/auth/logout'
  // ... other auth endpoints
}

// Files management endpoints
export const FILES_ENDPOINTS = {
  list: '/files'
  upload: '/files/upload'
  download: '/files/:id/download'
  // ... other file endpoints
}

// Analytics endpoints
export const ANALYTICS_ENDPOINTS = {
  stats: '/analytics/stats'
  activity: '/analytics/activity'
  // ... other analytics endpoints
}
```

## Setting Up the API URL

### Development Environment

1. Create a `.env.local` file in the project root:

```env
VITE_API_URL=http://localhost:3000/api/v1
```

2. The application will automatically use this URL when you run `npm run dev`

### Production Environment

For production builds, update the API URL:

```env
VITE_API_URL=https://api.nebulacloud.com/v1
```

### Using Different Environments

You can define endpoints for different environments:

```typescript
const API_CONFIG = {
  endpoints: {
    development: 'http://localhost:3000/api/v1',
    staging: 'https://staging-api.nebulacloud.com/v1',
    production: 'https://api.nebulacloud.com/v1'
  }
}
```

Get the appropriate URL using the helper function:

```typescript
import { getApiUrl } from '@/config/apiConfig'

const url = getApiUrl('production')
```

## API Endpoints Reference

### Authentication

#### Register User
```
POST /auth/register
Body: {
  firstName: string
  lastName: string
  email: string
  password: string
}
Response: {
  success: boolean
  message: string
  userId: string
}
```

#### Login
```
POST /auth/login
Body: {
  email: string
  password: string
}
Response: {
  success: boolean
  token: string
  user: {...}
}
```

#### Logout
```
POST /auth/logout
Headers: {
  Authorization: Bearer <token>
}
```

### Files Management

#### List Files
```
GET /files?parentId=root
Response: {
  files: FileItem[]
  totalSize: number
  count: number
}
```

#### Upload File
```
POST /files/upload
Body: FormData {
  file: File
  parentId: string
}
Response: {
  id: string
  name: string
  size: number
  // ... file details
}
```

#### Download File
```
GET /files/:id/download
Response: File binary data
```

#### Delete File
```
DELETE /files/:id
```

#### Search Files
```
GET /files/search?query=keyword
Response: {
  results: FileItem[]
  totalResults: number
}
```

### Folders

#### Create Folder
```
POST /folders
Body: {
  name: string
  parentId: string
}
Response: {
  id: string
  name: string
  // ... folder details
}
```

#### Delete Folder
```
DELETE /folders/:id
```

### Analytics

#### Get Storage Stats
```
GET /analytics/stats
Response: {
  totalFiles: number
  totalFolders: number
  totalSize: number
  filesByType: {...}
}
```

#### Get Activity Data
```
GET /analytics/activity?days=7
Response: {
  dates: string[]
  uploadCounts: number[]
}
```

## Storage Configuration

### Storage Limits

The application supports configurable storage quotas:

```typescript
export const STORAGE_CONFIG = {
  maxStorage: 10 * 1024 * 1024 * 1024,        // 10 GB
  warningThreshold: 0.8,                       // 80%
  criticalThreshold: 0.95,                     // 95%
  maxFileSize: 5 * 1024 * 1024 * 1024,        // 5 GB per file
  maxBatchUploadSize: 10 * 1024 * 1024 * 1024 // 10 GB batch
}
```

### Allowed File Types

The following MIME types are allowed by default:
- Documents: PDF, Word, Excel, Text, Markdown
- Images: JPEG, PNG, GIF
- Media: MP4, MPEG, MP3, FLAC
- Archives: ZIP, RAR, 7Z, TAR, GZ

Customize in `apiConfig.ts`:

```typescript
STORAGE_CONFIG.allowedFileTypes = [
  'application/pdf',
  'image/jpeg',
  // ... add more MIME types
]
```

## Authentication Integration

### User Registration

The app includes a complete registration flow with:
- Email validation
- Password strength requirements:
  - Minimum 8 characters
  - At least one uppercase letter
  - At least one number
  - At least one special character
- Email verification
- Terms and conditions acceptance

Configure registration settings in `apiConfig.ts`:

```typescript
export const REGISTRATION_CONFIG = {
  minPasswordLength: 8,
  requireUppercase: true,
  requireNumbers: true,
  requireSpecialChars: true,
  emailVerificationRequired: true,
  emailVerificationTimeout: 24 // hours
}
```

### User Login

- Email and password validation
- "Remember me" functionality
- Session management with localStorage
- Automatic logout on expiration

## Error Handling

The application includes predefined error messages in Russian:

```typescript
export const API_MESSAGES = {
  success: {
    registration: 'Регистрация успешна!',
    login: 'Успешный вход в систему!',
    fileUploaded: 'Файл успешно загружен.'
  },
  error: {
    invalidEmail: 'Пожалуйста, введите корректный адрес электронной почты.',
    weakPassword: 'Пароль не соответствует требованиям безопасности.',
    serverError: 'Ошибка сервера. Пожалуйста, попробуйте позже.'
  }
}
```

Customize error messages as needed.

## Rate Limiting

The API respects rate limits to prevent abuse:

```typescript
export const RATE_LIMITS = {
  requestsPerMinute: 60,
  uploadsPerHour: 100,
  downloadsPerHour: 200,
  searchPerMinute: 30
}
```

## Retry Configuration

Failed requests are automatically retried with exponential backoff:

```typescript
export const RETRY_CONFIG = {
  maxRetries: 3,
  retryDelay: 1000,                // 1 second
  backoffMultiplier: 2,             // Double delay each retry
  retryStatusCodes: [408, 429, 500, 502, 503, 504]
}
```

## Making API Requests

### Using the Configuration

To make API requests, use the helper functions and configuration:

```typescript
import {
  API_CONFIG,
  AUTH_ENDPOINTS,
  FILES_ENDPOINTS,
  buildEndpointUrl,
  getApiUrl,
  DEFAULT_HEADERS
} from '@/config/apiConfig'

// Build endpoint URL with parameters
const uploadUrl = buildEndpointUrl(FILES_ENDPOINTS.upload)

// Make fetch request
const response = await fetch(uploadUrl, {
  method: 'POST',
  headers: DEFAULT_HEADERS,
  body: JSON.stringify({ /* data */ })
})
```

## Development Notes

- All demo data is stored in `src/stores/mockData.ts`
- Authentication state is managed locally in `src/App.vue`
- User session is persisted in localStorage
- The app includes both Login and Register pages

## Migration from Demo to Real API

To switch from demo data to a real backend:

1. Update `.env` with your API URL
2. Replace mock data calls in components with actual API calls
3. Remove the mock file generators
4. Implement proper token management (JWT, OAuth, etc.)
5. Add error handling and loading states
6. Test all authentication flows

## Support

For API integration questions or issues, refer to:
- Main configuration: `src/config/apiConfig.ts`
- Types: `src/types/index.ts`
- Mock data: `src/stores/mockData.ts`
- Auth pages: `src/components/LoginPage.vue` and `src/components/RegisterPage.vue`
