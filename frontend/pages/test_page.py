import streamlit as st


st.set_page_config(
    page_title="Моё многостраничное приложение", page_icon="🚀", layout="wide"
)

# Инициализация шага
if "step" not in st.session_state:
    st.session_state.step = 1
if "form_data" not in st.session_state:
    st.session_state.form_data = {}

st.title("Многошаговая регистрация")

# Шаг 1: Основная информация
if st.session_state.step == 1:
    st.header("Шаг 1: Личные данные")

    name = st.text_input("Имя", value=st.session_state.form_data.get("name", ""))
    email = st.text_input("Email", value=st.session_state.form_data.get("email", ""))

    if st.button("Далее"):
        st.session_state.form_data["name"] = name
        st.session_state.form_data["email"] = email
        st.session_state.step = 2
        st.rerun()

# Шаг 2: Дополнительные настройки
elif st.session_state.step == 2:
    st.header("Шаг 2: Настройки")

    age = st.slider("Возраст", 18, 100, value=st.session_state.form_data.get("age", 25))
    newsletter = st.checkbox(
        "Подписаться на рассылку",
        value=st.session_state.form_data.get("newsletter", False),
    )

    col1, col2 = st.columns(2)
    with col1:
        if st.button("Назад"):
            st.session_state.step = 1
            st.rerun()
    with col2:
        if st.button("Далее"):
            st.session_state.form_data["age"] = age
            st.session_state.form_data["newsletter"] = newsletter
            st.session_state.step = 3
            st.rerun()

# Шаг 3: Подтверждение
elif st.session_state.step == 3:
    st.header("Шаг 3: Подтверждение")

    st.write("Проверьте введённые данные:")
    st.json(st.session_state.form_data)

    if st.button("Отправить"):
        st.success("Регистрация завершена! (тут был бы код сохранения в БД)")
        # Сбросить форму можно так:
        # st.session_state.step = 1
        # st.session_state.form_data = {}

    if st.button("Назад к шагу 2"):
        st.session_state.step = 2
        st.rerun()
