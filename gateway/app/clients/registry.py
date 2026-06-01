from app.clients.base import AsyncGatewayClient
from app.core.config import settings

class ClientRegistry:
    def __init__(self):
        self.auth = AsyncGatewayClient(settings.AUTH_SERVICE_URL)
        self.storage = AsyncGatewayClient(settings.STORAGE_SERVICE_URL)
        self.analysis = AsyncGatewayClient(settings.ANALYSIS_SERVICE_URL)
        self.encrypt = AsyncGatewayClient(settings.ENCRYPT_SERVICE_URL)

registry = ClientRegistry()

def get_auth_client(): return registry.auth
def get_storage_client(): return registry.storage
def get_analysis_client(): return registry.analysis
def get_encrypt_client(): return registry.encrypt
