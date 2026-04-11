import streamlit as st
import uuid
from datetime import datetime

# --- Инициализация состояния ---
if "authenticated" not in st.session_state:
    st.session_state.authenticated = False
if "current_user" not in st.session_state:
    st.session_state.current_user = None
if "files" not in st.session_state:
    # Хранилище файлов: список словарей {id, name, size, owner, uploaded_at}
    st.session_state.files = []

# --- Заглушка для пользователей ---
USERS = {
    "admin": {"password": "admin", "role": "admin"},
    "user": {"password": "user", "role": "user"},
}


# --- Функции-заглушки ---
def login_stub(username, password):
    if username in USERS and USERS[username]["password"] == password:
        st.session_state.authenticated = True
        st.session_state.current_user = username
        return True
    return False


def logout_stub():
    st.session_state.authenticated = False
    st.session_state.current_user = None


def upload_file_stub(uploaded_file):
    """Сохраняет метаданные файла в session_state (содержимое не храним)"""
    file_id = str(uuid.uuid4())
    st.session_state.files.append({
        "id": file_id,
        "name": uploaded_file.name,
        "size": len(uploaded_file.getvalue()),
        "owner": st.session_state.current_user,
        "uploaded_at": datetime.now().isoformat(),
    })
    return True


def get_user_files_stub():
    return [
        f for f in st.session_state.files if f["owner"] == st.session_state.current_user
    ]


def get_download_url_stub(file_id):
    # Просто имитация ссылки (не рабочая)
    return f"/fake-download/{file_id}"


# --- UI ---
st.set_page_config(page_title="Облачное хранилище (заглушка)", page_icon="☁️")

if not st.session_state.authenticated:
    st.title("☁️ Вход в облачное хранилище (демо)")
    with st.form("login_form"):
        username = st.text_input("Имя пользователя")
        password = st.text_input("Пароль", type="password")
        submitted = st.form_submit_button("Войти")
        if submitted:
            if login_stub(username, password):
                st.success("Успешный вход!")
                st.rerun()
            else:
                st.error("Неверное имя пользователя или пароль")
else:
    st.sidebar.title(f"Привет, {st.session_state.current_user}")
    if st.sidebar.button("Выйти"):
        logout_stub()
        st.rerun()

    st.title("Мои файлы")

    # Загрузка файла
    with st.expander("Загрузить файл"):
        uploaded_file = st.file_uploader("Выберите файл...")
        if uploaded_file and st.button("Загрузить"):
            upload_file_stub(uploaded_file)
            st.success("Файл загружен (заглушка)")
            st.rerun()

    # Список файлов
    files = get_user_files_stub()
    if files:
        for file in files:
            col1, col2, col3 = st.columns([3, 1, 1])
            col1.write(file["name"])
            col2.write(f"{file['size'] / 1024:.2f} KB")
            if col3.button("📥 Скачать", key=file["id"]):
                url = get_download_url_stub(file["id"])
                st.info(
                    f"Заглушка: ссылка для скачивания {url} (в реальности тут будет настоящая ссылка)"
                )
    else:
        st.info("У вас пока нет файлов. Загрузите первый!")
