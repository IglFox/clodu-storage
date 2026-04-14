import streamlit as st


class StorageView:
    def __init__(self, client):
        self.client = client

    def render(self):
        st.title("Файловое хранилище")
        token = st.session_state.get("token")
        if not token:
            st.warning("Сначала войдите")
            return

        # Загрузка файла
        uploaded = st.file_uploader("Выберите файл")
        if uploaded and st.button("Загрузить"):
            try:
                self.client.upload_file(token, uploaded.name, uploaded.getvalue())
                st.success("Файл загружен")
                st.rerun()
            except Exception as e:
                st.error(f"Ошибка загрузки: {e}")

        # Список файлов
        try:
            files = self.client.list_files(token)
        except Exception as e:
            st.error(f"Не удалось получить список файлов: {e}")
            return

        if not files:
            st.info("Нет загруженных файлов")
            return

        st.subheader("Загруженные файлы")
        for f in files:
            col1, col2 = st.columns([0.8, 0.2])
            col1.write(f["filename"])
            if col2.button("Скачать", key=f["id"]):
                try:
                    content = self.client.download_file(token, f["id"])
                    st.download_button(
                        "Сохранить",
                        content,
                        file_name=f["filename"],
                        key=f"dl_{f['id']}",
                    )
                except Exception as e:
                    st.error(f"Ошибка скачивания: {e}")
