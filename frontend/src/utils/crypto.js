export const deriveKey = (password) => {
  return {
    display: password,
    raw: btoa(password),
    strength: password.length > 8 ? 'High' : 'Medium'
  };
};

export const encryptData = (data, key) => {
  return `encrypted_${btoa(data)}`;
};

export const decryptData = (metadata, key) => {
  return atob(metadata.cipherText.replace('encrypted_', ''));
};

export const sha256 = (data) => {
  return 'sha256_' + Math.random().toString(36).substr(2, 8);
};
