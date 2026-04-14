import streamlit as st
from api_clients.gateway_client import GatewayClient
from views.login_view import LoginView
from views.storage_view import StorageView
from views.analysis_view import AnalysisView
from views.encrypt_view import EncryptView

# Инициализация клиента
if "gateway_client" not in st.session_state:
    st.session_state.gateway_client = GatewayClient()

client = st.session_state.gateway_client

# Проверка авторизации
token = st.session_state.get("token")
if token:
    try:
        if client.verify(token).get("valid"):
            st.sidebar.title("Меню")
            page = st.sidebar.radio("Страницы", ["Файлы", "Анализ", "Шифрование"])
            if st.sidebar.button("Выйти"):
                del st.session_state.token
                st.rerun()

            if page == "Файлы":
                StorageView(client).render()
            elif page == "Анализ":
                AnalysisView(client).render()
            elif page == "Шифрование":
                EncryptView(client).render()
        else:
            del st.session_state.token
            st.rerun()
    except:
        del st.session_state.token
        st.rerun()
else:
    LoginView(client).render()
