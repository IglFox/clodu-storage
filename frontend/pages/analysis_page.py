# iris_dashboard.py
import streamlit as st
import pandas as pd
import plotly.express as px

# 1. Настройка конфигурации страницы (должна быть самой первой командой Streamlit)
st.set_page_config(page_title="Анализ Ирисов Фишера", layout="wide")
st.title("📊 Интерактивный дашборд: Ирисы Фишера")
st.markdown(
    "Это приложение позволяет визуализировать знаменитый набор данных об ирисах."
)

# 2. Загружаем данные
# Для наглядности используем встроенный датасет из библиотеки plotly
df = px.data.iris()

# 3. Боковая панель с фильтрами
st.sidebar.header("Настройки фильтрации")
# Создаём мультивыбор для фильтрации по видам ирисов
selected_species = st.sidebar.multiselect(
    "Выберите вид ириса:",
    options=df["species"].unique(),
    default=df["species"].unique(),  # По умолчанию выбраны все
)

# 4. Применяем фильтр к данным
# Фильтруем DataFrame, оставляя только выбранные пользователем виды
filtered_df = df[df["species"].isin(selected_species)]

# 5. Отображаем ключевые метрики
# Используем колонки для красивого расположения
col1, col2, col3, col4 = st.columns(4)
with col1:
    st.metric("Всего записей", len(filtered_df))
with col2:
    st.metric("Уникальных видов", filtered_df["species"].nunique())
with col3:
    st.metric("Ср. длина чашелистика", round(filtered_df["sepal_length"].mean(), 2))
with col4:
    st.metric("Ср. ширина лепестка", round(filtered_df["petal_width"].mean(), 2))

# 6. Визуализация данных
st.subheader("Визуализация взаимосвязей")
# Создаём две колонки для графиков
col_chart1, col_chart2 = st.columns(2)

with col_chart1:
    # Точечный график: длина чашелистика vs ширина лепестка
    fig1 = px.scatter(
        filtered_df,
        x="sepal_length",
        y="petal_length",
        color="species",
        size="petal_width",
        title="Зависимость длины чашелистика от длины лепестка",
    )
    st.plotly_chart(fig1, use_container_width=True)

with col_chart2:
    # Гистограмма распределения длин чашелистиков
    fig2 = px.histogram(
        filtered_df,
        x="sepal_length",
        color="species",
        title="Распределение длины чашелистика",
    )
    st.plotly_chart(fig2, use_container_width=True)

# 7. Показываем отфильтрованные данные в виде таблицы
st.subheader("Отфильтрованные данные")
st.dataframe(filtered_df)
