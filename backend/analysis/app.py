from fastapi import FastAPI
from schemas.analysis import AnalysisResponse

app = FastAPI()


@app.post("/analyze", response_model=AnalysisResponse)
async def analyze(file_id: str):
    return AnalysisResponse(
        file_id=file_id,
        status="completed",
        result={
            "summary": "Mock analysis result",
            "word_count": 1234,
            "sentiment": "neutral",
        },
    )
