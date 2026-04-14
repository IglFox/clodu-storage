import streamlit as st

from api_clients.gateway_client import GatewayClient


class AnalysisView:
    def __init__(self, client: GatewayClient):
        self.client = client

    def render(self):
        st.title("Анализ данных")
        token = st.session_state.get("token")
        if not token:
            st.warning("Сначала войдите")
            return

        try:
            files = self.client.list_files(token)
        except Exception as e:
            st.error(f"Не удалось получить список файлов: {e}")
            return

        if not files:
            st.info("Нет файлов для анализа")
            return

        selected_file = st.selectbox(
            "Выберите файл", files, format_func=lambda f: f["filename"]
        )
        if st.button("Запустить анализ"):
            try:
                result = self.client.analyze(token, selected_file["id"])
                st.subheader("Результат анализа")
                st.json(result)
            except Exception as e:
                st.error(f"Ошибка анализа: {e}")
