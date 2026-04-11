import pandas as pd
import streamlit as st


@st.cache_data
def load_data_from_storage():
    # Здесь читаете из CSV, SQL, S3
    return pd.read_csv("data.csv")
