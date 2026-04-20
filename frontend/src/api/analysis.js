import apiClient from './client';

export async function fetchStorageStats() {
  // временно используем мок
  const files = (await import('./storage')).mockFiles || [];
  const totalBytes = files.filter(f => f.type === 'file').reduce((sum, f) => sum + f.size, 0);
  const totalGB = totalBytes / (1024 ** 3);
  const typeBreakdown = { documents: 0, images: 0, media: 0, archives: 0, other: 0 };
  files.forEach(f => {
    if (f.type !== 'file') return;
    const ext = f.name.split('.').pop().toLowerCase();
    const sizeGb = f.size / (1024 ** 3);
    if (['pdf','doc','docx','txt','md','xlsx'].includes(ext)) typeBreakdown.documents += sizeGb;
    else if (['jpg','jpeg','png','gif'].includes(ext)) typeBreakdown.images += sizeGb;
    else if (['mp4','mkv','mp3','wav'].includes(ext)) typeBreakdown.media += sizeGb;
    else if (['zip','rar','7z'].includes(ext)) typeBreakdown.archives += sizeGb;
    else typeBreakdown.other += sizeGb;
  });
  return { totalGB, typeBreakdown };
}
