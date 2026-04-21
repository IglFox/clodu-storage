export interface FileItem {
  id: string
  name: string
  type: 'file' | 'folder'
  parentId: string | null
  size: number // in bytes
  modified: Date
  contentPreview?: string
  tags: string[]
  extension?: string
}

export interface SearchResult extends FileItem {
  matchType: 'name' | 'extension' | 'content' | 'tag'
  relevance: number
  highlights?: string[]
}

export interface FileStats {
  totalFiles: number
  totalFolders: number
  totalSize: number
  filesByType: {
    [key: string]: {
      count: number
      size: number
      displayName: string
      icon: string
    }
  }
}
