import streamlit as st
from schemas.Schemas import User
import requests
from logging import get_logger


_logger = get_logger(__name__)


class LoginManager:
    def __init__(self):
        if "authenticated" not in st.session_state:
            st.session_state.authenticated = False

    def is_user_have_account(self, username: str) -> bool:
        try:
            request = requests.get(f"http://auth/users/{username}")
        except Exception:
            return False
        return request.status_code == 200

    def login(self, username: str, password: str) -> str:
        if st.session_state.authenticated:
            return "already_authenticated"

        if self.is_user_have_account(username):
            return "account_exists"

        try:
            request = requests.post(
                "http://auth/login", json={"username": username, "password": password}
            )
        except Exception:
            return "failed"

        if request.status_code == 200:
            st.session_state.authenticated = True
            return "sucess"

        return "failed"
