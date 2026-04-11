import streamlit as st
import uuid
from datetime import datetime
import networkx as nx
from pyvis.network import Network
import tempfile
import json

# --- Инициализация состояния ---
if "authenticated" not in st.session_state:
    st.session_state.authenticated = False
if "current_user" not in st.session_state:
    st.session_state.current_user = None
if "files" not in st.session_state:
    st.session_state.files = []
if "show_graph" not in st.session_state:  # флаг для отображения графа
    st.session_state.show_graph = False

# --- Заглушка для пользователей ---
USERS = {
    "admin": {"password": "admin", "role": "admin"},
    "user": {"password": "user", "role": "user"},
}


# --- Функции-заглушки (без изменений) ---
def login_stub(username, password):
    if username in USERS and USERS[username]["password"] == password:
        st.session_state.authenticated = True
        st.session_state.current_user = username
        return True
    return False


def logout_stub():
    st.session_state.authenticated = False
    st.session_state.current_user = None
    st.session_state.show_graph = False


def upload_file_stub(uploaded_file):
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
    return f"/fake-download/{file_id}"


# --- Функция для создания демо-графа (заглушка) ---
def build_demo_graph():
    """Создаёт пример графа заметок (как в Obsidian)"""
    G = nx.Graph()
    # Добавим несколько узлов и связей
    edges = [
        ("Проект Alpha", "Заметки по проекту"),
        ("Проект Alpha", "Встречи"),
        ("Заметки по проекту", "Идеи"),
        ("Встречи", "План действий"),
        ("Идеи", "Реализация"),
        ("Реализация", "Деплой"),
        ("Личные заметки", "Идеи"),
    ]
    G.add_edges_from(edges)
    return G


def render_graph_in_streamlit(G):
    """Принимает граф NetworkX и отображает его через pyvis в Streamlit"""
    net = Network(height="600px", width="100%", bgcolor="#1e1e1e", font_color="white")

    # Настройки внешнего вида
    options = {
        "nodes": {
            "shape": "dot",
            "size": 20,
            "font": {"size": 14, "face": "Arial"},
            "borderWidth": 1,
            "shadow": True,
        },
        "edges": {
            "color": "#848484",
            "width": 1.5,
            "smooth": {"enabled": True, "type": "continuous"},
        },
        "physics": {
            "enabled": True,
            "solver": "forceAtlas2Based",
            "forceAtlas2Based": {
                "gravitationalConstant": -50,
                "centralGravity": 0.01,
                "springLength": 120,
            },
            "stabilization": {"iterations": 150},
        },
        "interaction": {
            "hover": True,
            "tooltipDelay": 100,
            "zoomView": True,
            "dragView": True,
            "navigationButtons": True,
        },
    }
    net.set_options(json.dumps(options))

    # Добавляем узлы с размером, зависящим от степени
    for node in G.nodes():
        size = 15 + G.degree(node) * 3
        net.add_node(node, label=node, size=size, title=node)
    for edge in G.edges():
        net.add_edge(edge[0], edge[1])

    # Сохраняем во временный файл и отображаем
    with tempfile.NamedTemporaryFile(delete=False, suffix=".html") as tmp:
        net.save_graph(tmp.name)
        with open(tmp.name, "r", encoding="utf-8") as f:
            html_content = f.read()
        st.components.v1.html(html_content, height=650)


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

    # --- НОВЫЙ РАЗДЕЛ В БОКОВОЙ ПАНЕЛИ: ГРАФ ---
    st.sidebar.markdown("---")
    st.sidebar.subheader("📊 Граф заметок")
    if st.sidebar.button("🔍 Показать граф (заглушка)"):
        st.session_state.show_graph = not st.session_state.show_graph  # переключатель
        # если нужно всегда показывать при нажатии, можно st.session_state.show_graph = True
        # но для удобства сделаем toggle
    st.sidebar.caption(
        "Нажмите, чтобы отобразить интерактивный граф связей между заметками (демо-пример)"
    )

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

    # --- Отображение графа, если включено ---
    if st.session_state.show_graph:
        st.markdown("---")
        st.subheader("📈 Граф связей заметок (демо-пример)")
        demo_graph = build_demo_graph()
        render_graph_in_streamlit(demo_graph)
        st.caption(
            "Это заглушка – граф построен на основе примера. В реальном приложении вы можете загружать свои .md файлы."
        )
