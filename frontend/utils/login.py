import streamlit as st


def check_login():
    if "authenticated" not in st.session_state:
        st.session_state.authenticated = False

    if not st.session_state.authenticated:
        with st.form("login"):
            pwd = st.text_input("Пароль", type="password")
            if st.form_submit_button("Войти"):
                if pwd == "secret":
                    st.session_state.authenticated = True
                    st.rerun()
                else:
                    st.error("Неверный пароль")
        return False
    return True
