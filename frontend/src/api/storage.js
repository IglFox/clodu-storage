import apiClient from './client';

// Мок-данные для демонстрации (можно удалить, когда будет бэк)
let mockFiles = [
  { id: 'root', name: 'root', type: 'folder', parentId: null, size: 0, modified: Date.now() },
  { id: 'f1', name: 'Документы', type: 'folder', parentId: 'root', size: 0, modified: Date.now() - 86400000 },
  { id: 'f2', name: 'Фото', type: 'folder', parentId: 'root', size: 0, modified: Date.now() - 172800000 },
  { id: 'file1', name: 'отчет_2025.pdf', type: 'file', parentId: 'f1', size: 2.4 * 1024 * 1024, modified: Date.now() - 300000 },
  { id: 'file2', name: 'project_idea.md', type: 'file', parentId: 'f1', size: 120 * 1024, modified: Date.now() - 86400000 },
  { id: 'img1', name: 'sunset.jpg', type: 'file', parentId: 'f2', size: 3.2 * 1024 * 1024, modified: Date.now() - 43200000 },
  { id: 'rootfile', name: 'welcome.txt', type: 'file', parentId: 'root', size: 45 * 1024, modified: Date.now() - 600000 },
];

const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));

export async function fetchAllFiles() {
  await delay(300);
  return [...mockFiles];
}

export async function createFolder(name, parentId) {
  await delay(400);
  const newId = Date.now() + '-' + Math.random().toString(36);
  const newFolder = { id: newId, name, type: 'folder', parentId, size: 0, modified: Date.now() };
  mockFiles.push(newFolder);
  return newFolder;
}

export async function deleteItem(id) {
  await delay(300);
  const toDelete = [id];
  const findChildren = (parentId) => {
    mockFiles.forEach(f => {
      if (f.parentId === parentId && !toDelete.includes(f.id)) {
        toDelete.push(f.id);
        if (f.type === 'folder') findChildren(f.id);
      }
    });
  };
  const item = mockFiles.find(f => f.id === id);
  if (item?.type === 'folder') findChildren(id);
  mockFiles = mockFiles.filter(f => !toDelete.includes(f.id));
  return { success: true };
}

export async function uploadFile(file, parentId) {
  await delay(600);
  const newId = Date.now() + '-' + Math.random().toString(36);
  const newFile = {
    id: newId,
    name: file.name,
    type: 'file',
    parentId,
    size: file.size,
    modified: Date.now(),
  };
  mockFiles.push(newFile);
  return newFile;
}
