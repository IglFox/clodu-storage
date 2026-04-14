import streamlit as st
from utils.auth import check_login

st.set_page_config(page_title="My App")

if not check_login():
    st.stop()
st.title("Добро пожаловать")
