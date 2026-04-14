import streamlit as st


class EncryptView:
    def __init__(self, client):
        self.client = client

    def render(self):
        st.title("Шифрование / Дешифрование")
        token = st.session_state.get("token")
        if not token:
            st.warning("Сначала войдите")
            return

        uploaded = st.file_uploader("Файл для преобразования")
        if uploaded:
            data = uploaded.getvalue()
            mode = st.radio("Режим", ("Зашифровать", "Расшифровать"))
            if st.button("Выполнить"):
                try:
                    if mode == "Зашифровать":
                        result = self.client.encrypt(token, data)
                        st.download_button(
                            "Скачать зашифрованное", result, "encrypted.bin"
                        )
                    else:
                        result = self.client.decrypt(token, data)
                        st.download_button(
                            "Скачать расшифрованное", result, "decrypted"
                        )
                except Exception as e:
                    st.error(f"Ошибка: {e}")
