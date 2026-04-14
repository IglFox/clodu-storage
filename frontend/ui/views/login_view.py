import streamlit as st


class LoginView:
    def __init__(self, client):
        self.client = client

    def render(self):
        st.title("Вход в систему")
        username = st.text_input("Логин")
        password = st.text_input("Пароль", type="password")
        if st.button("Войти"):
            if not username or not password:
                st.error("Введите логин и пароль")
                return
            try:
                resp = self.client.login(username, password)
                st.session_state.token = resp["token"]
                st.success("Успешный вход!")
                st.rerun()
            except Exception as e:
                st.error(f"Ошибка: {e}")
