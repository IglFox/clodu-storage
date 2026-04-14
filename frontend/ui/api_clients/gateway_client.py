import os
import requests
from typing import Optional, Dict, Any


class GatewayClient:
    def __init__(self, base_url: str = None):
        if base_url is None:
            base_url = os.getenv("GATEWAY_URL", "http://gateway:8000")
        self.base_url = base_url

    def _request(
        self, method: str, path: str, token: Optional[str] = None, **kwargs
    ) -> Any:
        url = f"{self.base_url}{path}"
        headers = {}
        if token:
            headers["Authorization"] = f"Bearer {token}"
        if "json" in kwargs:
            headers["Content-Type"] = "application/json"
        response = requests.request(method, url, headers=headers, **kwargs)
        response.raise_for_status()
        if response.content:
            try:
                return response.json()
            except:
                return response.content
        return None

    # Auth
    def login(self, username: str, password: str) -> Dict:
        return self._request(
            "POST", "/auth/login", json={"username": username, "password": password}
        )

    def verify(self, token: str) -> Dict:
        return self._request("POST", "/auth/verify", json={"token": token})

    # Storage
    def upload_file(self, token: str, filename: str, content: bytes) -> Dict:
        files = {"file": (filename, content)}
        return self._request("POST", "/storage/upload", token=token, files=files)

    def list_files(self, token: str) -> list:
        return self._request("GET", "/storage/list", token=token)

    def download_file(self, token: str, file_id: str) -> bytes:
        url = f"{self.base_url}/storage/download/{file_id}"
        headers = {"Authorization": f"Bearer {token}"}
        response = requests.get(url, headers=headers)
        response.raise_for_status()
        return response.content

    # Analysis
    def analyze(self, token: str, file_id: str) -> Dict:
        return self._request(
            "POST", f"/analysis/analyze", token=token, params={"file_id": file_id}
        )

    # Encrypt
    def encrypt(self, token: str, data: bytes) -> bytes:
        files = {"data": data}
        return self._request("POST", "/encrypt/encrypt", token=token, files=files)

    def decrypt(self, token: str, data: bytes) -> bytes:
        files = {"data": data}
        return self._request("POST", "/encrypt/decrypt", token=token, files=files)
