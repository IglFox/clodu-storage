import os
import re
import networkx as nx
from pyvis.network import Network
import json


# ===== 1. Укажите путь к вашей папке с заметками Obsidian =====
VAULT_PATH = r"C:\Users\antma\Documents\my obsidian vault"  # измените под себя


# ===== 2. Функция для извлечения всех [[wikilinks]] из текста =====
def extract_wikilinks(text):
    # Ищем шаблон [[...]] , но не захватываем вложенные скобки
    # Поддерживаем алиасы [[Заметка|текст]] и секции [[Заметка#заголовок]]
    pattern = r"\[\[(.*?)\]\]"
    raw_links = re.findall(pattern, text)
    clean_links = []
    for link in raw_links:
        # Обрезаем алиас после | и якорь после #
        clean = link.split("|")[0].split("#")[0].strip()
        if clean:
            clean_links.append(clean)
    return clean_links


# ===== 3. Строим граф =====
G = nx.Graph()  # неориентированный граф (как в Obsidian)
# Если нужны направления (кто на кого ссылается), используйте nx.DiGraph()

# Проходим по всем .md файлам в папке (не рекурсивно, только верхний уровень)
for filename in os.listdir(VAULT_PATH):
    if not filename.endswith(".md"):
        continue

    filepath = os.path.join(VAULT_PATH, filename)
    with open(filepath, "r", encoding="utf-8") as f:
        content = f.read()

    current_note = filename[:-3]  # убираем .md
    G.add_node(current_note)

    links = extract_wikilinks(content)
    for target in links:
        # Не добавляем ссылки на самого себя (обычно в Obsidian они не рисуются)
        if target == current_note:
            continue
        G.add_node(target)
        G.add_edge(current_note, target)

print(f"Построен граф: {G.number_of_nodes()} узлов, {G.number_of_edges()} рёбер")

# ===== 4. Настройка визуализации через Pyvis =====
net = Network(
    height="750px",
    width="100%",
    bgcolor="#1e1e1e",  # тёмный фон, как в Obsidian
    font_color="white",
)

# Настраиваем физику и внешний вид под Obsidian
# options = {
#     "nodes": {
#         "shape": "dot",
#         "size": 20,
#         "font": {"size": 14, "face": "Arial"},
#         "borderWidth": 1,
#         "shadow": True,
#     },
#     "edges": {
#         "color": {"color": "#848484", "highlight": "#ffffff", "hover": "#ffffff"},
#         "width": 1.5,
#         "smooth": {"enabled": True, "type": "continuous"},
#     },
#     "physics": {
#         "enabled": True,
#         "solver": "forceAtlas2Based",
#         "forceAtlas2Based": {
#             "gravitationalConstant": -50,
#             "centralGravity": 0.01,
#             "springLength": 120,
#             "springConstant": 0.08,
#             "damping": 0.4,
#             "avoidOverlap": 0.5,
#         },
#         "stabilization": {"iterations": 150},
#     },
#     "interaction": {
#         "hover": True,
#         "tooltipDelay": 100,
#         "zoomView": True,
#         "dragView": True,
#         "navigationButtons": True,
#     },
# }

# net = Network(height="750px", width="100%", bgcolor="#1e1e1e", font_color="white")
# net.set_options(json.dumps(options))  # преобразуем словарь в JSON

# Добавляем узлы и рёбра
for node in G.nodes():
    size = 10 + G.degree(node) * 3
    net.add_node(node, label=node, size=size, title=node)
for edge in G.edges():
    net.add_edge(edge[0], edge[1])

net.show("obsidian_like_graph.html", notebook=False)
