import asyncio
from pathlib import Path
import pypdfium2  # pip install pypdfium2
import docx2txt
import io


async def extract_text(file_path: str, content_type: str) -> str:
    loop = asyncio.get_event_loop()
    return await loop.run_in_executor(None, _sync_extract, file_path, content_type)


def _sync_extract(file_path, content_type):
    if content_type == "application/pdf":
        pdf = pypdfium2.PdfDocument(file_path)
        text_parts = []
        for page in pdf:
            textpage = page.get_textpage()
            text_parts.append(textpage.get_text_range())
            textpage.close()
            page.close()
        pdf.close()
        return "\n".join(text_parts)

    if content_type in (
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/msword",
    ):
        return docx2txt.process(file_path)

    if content_type and content_type.startswith("text/"):
        return Path(file_path).read_text(encoding="utf-8", errors="ignore")

    return ""
