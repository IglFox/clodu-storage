import logging
import os

LEVEL = os.getenv("LOG_LEVEL", "INFO")


def get_logger(name: str):
    logger = logging.getLogger(name)
    logger.setLevel(LEVEL)
    if logger.handlers:
        return logger

    formatter = logging.Formatter("%(asctime)s | %(levelname)s | %(message)s")
    console_handler = logging.StreamHandler()
    console_handler.setFormatter(formatter)
    logger.addHandler(console_handler)

    return logger
