FROM python:3.11-slim

WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

RUN pip install streamlit networkx pyvis

COPY . .

ENTRYPOINT ["sh", "-c", "streamlit run pages/test_page.py"]
