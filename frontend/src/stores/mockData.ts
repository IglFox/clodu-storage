import type { FileItem } from '../types'

const ROOT_ID = 'root'
const DOCS_ID = 'docs'
const PROJECTS_ID = 'projects'
const PERSONAL_ID = 'personal'

export const mockFiles: FileItem[] = [
  // Root folders
  {
    id: DOCS_ID,
    name: 'Documents',
    type: 'folder',
    parentId: ROOT_ID,
    size: 0,
    modified: new Date('2024-01-15'),
    tags: ['folder', 'work'],
  },
  {
    id: PROJECTS_ID,
    name: 'Projects',
    type: 'folder',
    parentId: ROOT_ID,
    size: 0,
    modified: new Date('2024-01-18'),
    tags: ['folder', 'development'],
  },
  {
    id: PERSONAL_ID,
    name: 'Personal',
    type: 'folder',
    parentId: ROOT_ID,
    size: 0,
    modified: new Date('2024-01-10'),
    tags: ['folder', 'personal'],
  },

  // Files in Documents folder
  {
    id: 'f1',
    name: 'Q4_Report.pdf',
    type: 'file',
    parentId: DOCS_ID,
    size: 2.5 * 1024 * 1024,
    modified: new Date('2024-01-15'),
    extension: 'pdf',
    tags: ['report', 'financial', 'quarterly'],
    contentPreview: 'Quarterly financial report for Q4 2024. Contains detailed analysis of revenue streams...',
  },
  {
    id: 'f2',
    name: 'Budget_2024.xlsx',
    type: 'file',
    parentId: DOCS_ID,
    size: 1.2 * 1024 * 1024,
    modified: new Date('2024-01-12'),
    extension: 'xlsx',
    tags: ['budget', 'financial', 'planning'],
    contentPreview: 'Annual budget plan with expense categories and allocation by department...',
  },
  {
    id: 'f3',
    name: 'Meeting_Notes.md',
    type: 'file',
    parentId: DOCS_ID,
    size: 85 * 1024,
    modified: new Date('2024-01-18'),
    extension: 'md',
    tags: ['meeting', 'notes', 'action-items'],
    contentPreview:
      '# Team Standup - January 18\n\n## Agenda\n- Project status updates\n- Q1 planning\n- Resource allocation\n\n## Action Items\n1. Complete design mockups by Friday...',
  },

  // Files in Projects folder
  {
    id: 'f4',
    name: 'WebApp_v2.0.zip',
    type: 'file',
    parentId: PROJECTS_ID,
    size: 125 * 1024 * 1024,
    modified: new Date('2024-01-17'),
    extension: 'zip',
    tags: ['project', 'archive', 'code'],
    contentPreview: 'Complete source code archive for WebApp version 2.0 release...',
  },
  {
    id: 'f5',
    name: 'Design_System.figma',
    type: 'file',
    parentId: PROJECTS_ID,
    size: 45 * 1024 * 1024,
    modified: new Date('2024-01-16'),
    extension: 'figma',
    tags: ['design', 'ui', 'component-library'],
    contentPreview: 'Complete design system with components, patterns, and guidelines...',
  },
  {
    id: 'f6',
    name: 'Tutorial_Video.mp4',
    type: 'file',
    parentId: PROJECTS_ID,
    size: 250 * 1024 * 1024,
    modified: new Date('2024-01-14'),
    extension: 'mp4',
    tags: ['video', 'tutorial', 'education'],
    contentPreview: 'Step-by-step tutorial on using the new features of WebApp v2.0...',
  },

  // Files in Personal folder
  {
    id: 'f7',
    name: 'Vacation_Photos.zip',
    type: 'file',
    parentId: PERSONAL_ID,
    size: 2.8 * 1024 * 1024 * 1024,
    modified: new Date('2024-01-10'),
    extension: 'zip',
    tags: ['photos', 'personal', 'memories'],
    contentPreview: 'Collection of vacation photos from summer 2023 trip...',
  },
  {
    id: 'f8',
    name: 'Recipe_Collection.txt',
    type: 'file',
    parentId: PERSONAL_ID,
    size: 45 * 1024,
    modified: new Date('2024-01-09'),
    extension: 'txt',
    tags: ['personal', 'recipes', 'cooking'],
    contentPreview:
      'Personal collection of favorite recipes\n\n1. Pasta Carbonara\n2. Thai Green Curry\n3. Homemade Bread\n4. Chocolate Cake...',
  },
  {
    id: 'f9',
    name: 'Backup_Database.sql',
    type: 'file',
    parentId: PERSONAL_ID,
    size: 512 * 1024 * 1024,
    modified: new Date('2024-01-08'),
    extension: 'sql',
    tags: ['backup', 'database', 'important'],
    contentPreview: 'Full database backup from January 8, 2024...',
  },
  {
    id: 'f10',
    name: 'Family_Tree.jpg',
    type: 'file',
    parentId: PERSONAL_ID,
    size: 3.5 * 1024 * 1024,
    modified: new Date('2024-01-05'),
    extension: 'jpg',
    tags: ['family', 'photo', 'personal'],
    contentPreview: 'Family tree diagram showing relationships and generations...',
  },
]

// Helper functions
export function getFilesByParent(parentId: string): FileItem[] {
  return mockFiles.filter((f) => f.parentId === parentId)
}

export function getFileById(id: string): FileItem | undefined {
  return mockFiles.find((f) => f.id === id)
}

export function getAllFiles(): FileItem[] {
  return [...mockFiles]
}

export function getParentPath(fileId: string): string[] {
  const path: string[] = []
  let currentId: string | null = fileId

  while (currentId) {
    const file = getFileById(currentId)
    if (!file) break
    if (file.parentId !== ROOT_ID) {
      path.unshift(file.name)
    }
    currentId = file.parentId
  }

  return path
}

export function getTotalSize(): number {
  return mockFiles.reduce((sum, file) => sum + file.size, 0)
}

export function getFileStats() {
  const stats = {
    Documents: { count: 0, size: 0, displayName: 'Documents', icon: '📄' },
    Images: { count: 0, size: 0, displayName: 'Images', icon: '🖼️' },
    Media: { count: 0, size: 0, displayName: 'Media', icon: '🎬' },
    Archives: { count: 0, size: 0, displayName: 'Archives', icon: '📦' },
    Other: { count: 0, size: 0, displayName: 'Other', icon: '📋' },
  }

  mockFiles.forEach((file) => {
    if (file.type === 'file') {
      const ext = file.extension?.toLowerCase() || ''

      if (['pdf', 'doc', 'docx', 'txt', 'md', 'xlsx', 'xls', 'csv', 'ppt', 'pptx'].includes(ext)) {
        stats.Documents.count++
        stats.Documents.size += file.size
      } else if (['jpg', 'jpeg', 'png', 'gif', 'bmp', 'svg', 'webp'].includes(ext)) {
        stats.Images.count++
        stats.Images.size += file.size
      } else if (['mp4', 'avi', 'mov', 'mkv', 'flv', 'wmv', 'mp3', 'wav', 'flac'].includes(ext)) {
        stats.Media.count++
        stats.Media.size += file.size
      } else if (['zip', 'rar', '7z', 'tar', 'gz', 'bz2'].includes(ext)) {
        stats.Archives.count++
        stats.Archives.size += file.size
      } else {
        stats.Other.count++
        stats.Other.size += file.size
      }
    }
  })

  return stats
}

export function getLargeFiles(minSizeBytes = 50 * 1024 * 1024): FileItem[] {
  return mockFiles.filter((f) => f.type === 'file' && f.size >= minSizeBytes).sort((a, b) => b.size - a.size)
}

export function searchFiles(query: string, currentFolderId?: string): FileItem[] {
  const searchLower = query.toLowerCase()
  const results: Array<{ file: FileItem; score: number }> = []

  const fileList = currentFolderId ? getFilesByParent(currentFolderId) : mockFiles

  fileList.forEach((file) => {
    let score = 0

    // Name match (highest priority)
    if (file.name.toLowerCase().includes(searchLower)) {
      score += 100
      // Boost if it starts with search term
      if (file.name.toLowerCase().startsWith(searchLower)) {
        score += 50
      }
    }

    // Extension match
    if (file.extension && file.extension.toLowerCase().includes(searchLower)) {
      score += 50
    }

    // Tag match
    if (file.tags.some((tag) => tag.toLowerCase().includes(searchLower))) {
      score += 30
    }

    // Content match (lowest priority)
    if (file.contentPreview && file.contentPreview.toLowerCase().includes(searchLower)) {
      score += 10
    }

    if (score > 0) {
      results.push({ file, score })
    }
  })

  // Sort by relevance score
  return results.sort((a, b) => b.score - a.score).map((r) => r.file)
}
