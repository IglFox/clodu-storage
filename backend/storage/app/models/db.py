import uuid
from sqlalchemy import Column, String, DateTime, Integer, Text
from sqlalchemy.dialects.postgresql import UUID
from sqlalchemy.sql import func
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()


class FileMetadata(Base):
    __tablename__ = "files"

    file_id = Column(UUID(as_uuid=True), primary_key=True, default=uuid.uuid4)
    original_filename = Column(String(255), nullable=False)
    storage_path = Column(String(512), nullable=False)
    content_type = Column(String(100))
    size = Column(Integer)
    uploaded_at = Column(DateTime(timezone=True), server_default=func.now())
    extracted_text = Column(Text, nullable=True)  # кэш текста для быстрого превью
