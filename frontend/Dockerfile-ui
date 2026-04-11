FROM python:3.11-slim

WORKDIR /app

RUN pip install streamlit networkx pyvis

COPY . .

ENTRYPOINT ["sh", "-c", "streamlit run services/main_page.py"]
