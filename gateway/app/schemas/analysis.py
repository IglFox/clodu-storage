from pydantic import BaseModel
from typing import Dict, Any


class AnalysisResponse(BaseModel):
    file_id: str
    status: str
    result: Dict[str, Any]
