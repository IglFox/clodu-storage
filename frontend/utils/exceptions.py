from functools import wraps
from logger import get_logger

_logger = get_logger(__name__)


def handle_exceptions(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        try:
            return func(*args, **kwargs)
        except Exception as e:
            _logger.error(f"Exception in {func.__name__}: {e}")
            raise

    return wrapper
