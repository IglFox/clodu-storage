use axum::{http::StatusCode, response::{IntoResponse, Response}, Json};
use serde_json::json;

#[derive(Debug, thiserror::Error)]
pub enum AppError {
    #[error("Database error: {0}")]
    Database(#[from] rusqlite::Error),

    #[error("User already exists")]
    UserAlreadyExists,

    #[error("Invalid credentials")]
    InvalidCredentials,

    #[error("Invalid token")]
    InvalidToken,

    #[error("Missing authorization header")]
    MissingAuthHeader,

    #[error("Invalid authorization header format")]
    InvalidAuthHeader,
}

impl IntoResponse for AppError {
    fn into_response(self) -> Response {
        let (status, message) = match self {
            AppError::Database(_) => (StatusCode::INTERNAL_SERVER_ERROR, "Internal server error"),
            AppError::UserAlreadyExists => (StatusCode::CONFLICT, "User already exists"),
            AppError::InvalidCredentials => (StatusCode::UNAUTHORIZED, "Invalid email or password"),
            AppError::InvalidToken => (StatusCode::UNAUTHORIZED, "Invalid or expired token"),
            AppError::MissingAuthHeader => (StatusCode::UNAUTHORIZED, "Missing authorization header"),
            AppError::InvalidAuthHeader => (StatusCode::BAD_REQUEST, "Invalid authorization header"),
        };

        let body = Json(json!({ "error": message }));
        (status, body).into_response()
    }
}