export function formatSize(bytes) {
  if (bytes === 0) return '0 B';
  const k = 1024;
  const sizes = ['B', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return (bytes / Math.pow(k, i)).toFixed(1) + ' ' + sizes[i];
}

export function formatDate(timestamp) {
  return new Date(timestamp).toLocaleDateString();
}

export function getFileIcon(filename) {
  const ext = filename.split('.').pop().toLowerCase();
  if (['jpg','jpeg','png','gif','webp'].includes(ext)) return '🖼️';
  if (['mp4','mkv','mov'].includes(ext)) return '🎬';
  if (['mp3','wav','flac'].includes(ext)) return '🎵';
  if (['pdf','doc','docx','txt','md'].includes(ext)) return '📄';
  if (['zip','rar','7z','tar'].includes(ext)) return '🗜️';
  return '📄';
}
