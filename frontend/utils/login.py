import streamlit as st
from frontend.utils.exceptions import handle_exceptions
from schemas.Schemas import User
import requests
from logger import get_logger


_logger = get_logger(__name__)


class LoginManager:
    def __init__(self):
        if "authenticated" not in st.session_state:
            st.session_state.authenticated = False

    @handle_exceptions
    def is_user_have_account(self, username: str) -> bool:
        request = requests.get(f"http://auth/users/{username}")
        return request.status_code == 200

    @handle_exceptions
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
            _logger.error(f"Failed to login for user {username}")
            return "failed"

        if request.status_code == 200:
            st.session_state.authenticated = True
            return "sucess"

        return "failed"
