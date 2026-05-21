/**
 * AuthService
 * Handles the logic for requests to the external Authorization Microservice.
 */

export const AuthService = {
    /**
     * Sends an authentication request to the microservice (login or registration)
     * @param {string} baseUrl - Base URL of the auth microservice (e.g. 'http://localhost:8080')
     * @param {'login'|'register'} action - Action to perform
     * @param {string} email - User email address
     * @param {string} password - User password
     * @returns {Promise<{token: string, user?: any}>} The microservice response payload
     */
    async authenticate(baseUrl, action, email, password) {
        if (!baseUrl) {
            throw new Error("Microservice API URL is not defined.");
        }

        const endpoint = action === "register" ? "/register" : "/login";
        const cleanUrl = baseUrl.endsWith("/") ? baseUrl.slice(0, -1) : baseUrl;

        const response = await fetch(`${cleanUrl}${endpoint}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Accept: "application/json",
            },
            body: JSON.stringify({ email, password }),
        });

        if (!response.ok) {
            const errData = await response.json().catch(() => ({}));
            throw new Error(
                errData.message ||
                    errData.error ||
                    `Server returned error status ${response.status}`,
            );
        }

        return await response.json().catch(() => ({}));
    },

    /**
     * Retrieves the stored URL for the microservice endpoint
     * @returns {string}
     */
    getApiUrl() {
        return (
            localStorage.getItem("customAuthApiUrl") ||
            import.meta.env.AUTH_API_URL ||
            ""
        );
    },

    /**
     * Persists the microservice endpoint URL to localStorage
     * @param {string} url
     */
    setApiUrl(url) {
        if (url) {
            localStorage.setItem("customAuthApiUrl", url.trim());
        } else {
            localStorage.removeItem("customAuthApiUrl");
        }
    },
};
