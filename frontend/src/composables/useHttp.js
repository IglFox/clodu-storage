// src/composables/useHttp.js
import { API_BASE_URL } from "@/config/api";

/**
 * Универсальная обёртка над fetch.
 * Добавляет базовый URL, общие заголовки и обработку ошибок.
 */
export async function httpGet(path, opts = {}) {
    const url = `${API_BASE_URL}${path}`;
    const response = await fetch(url, {
        method: "GET",
        credentials: "include",
        headers: {
            "Content-Type": "application/json",
            ...(localStorage.getItem("accessToken")
                ? {
                      Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
                  }
                : {}),
            ...opts.headers,
        },
        ...opts,
    });

    if (!response.ok) {
        const err = await response.json().catch(() => ({}));
        throw new Error(err.message || `HTTP ${response.status}`);
    }

    // Если нужен бинарный ответ – возвращаем blob
    if (opts.responseType === "blob") {
        return response.blob();
    }

    return response.json();
}
