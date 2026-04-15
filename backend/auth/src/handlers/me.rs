use axum::{
    extract::State,
    http::{Request, header::AUTHORIZATION},
    middleware::Next,
    response::Response,
    Json,
};
use jsonwebtoken::{decode, DecodingKey, Validation};

use crate::{db::DbPool, models::{Claims, UserResponse}, error::AppError};

const JWT_SECRET: &[u8] = b"change_this_to_env_variable_in_production";

pub async fn me(
    claims: Claims,
    State(_pool): State<DbPool>,
) -> Result<Json<UserResponse>, AppError> {
    Ok(Json(UserResponse {
        id: claims.sub,
        email: claims.email,
    }))
}

pub async fn auth_middleware(
    State(_pool): State<DbPool>,
    mut req: Request<axum::body::Body>,
    next: Next,
) -> Result<Response, AppError> {
    let auth_header = req
        .headers()
        .get(AUTHORIZATION)
        .ok_or(AppError::MissingAuthHeader)?
        .to_str()
        .map_err(|_| AppError::InvalidAuthHeader)?;

    let token = auth_header
        .strip_prefix("Bearer ")
        .ok_or(AppError::InvalidAuthHeader)?;

    let claims = decode::<Claims>(
        token,
        &DecodingKey::from_secret(JWT_SECRET),
        &Validation::default(),
    )
    .map_err(|_| AppError::InvalidToken)?
    .claims;

    req.extensions_mut().insert(claims);
    Ok(next.run(req).await)
}

// Извлечение Claims из extensions для handler'а me
impl axum::extract::FromRequestParts<DbPool> for Claims {
    type Rejection = AppError;

    async fn from_request_parts(
        parts: &mut axum::http::request::Parts,
        _state: &DbPool,
    ) -> Result<Self, Self::Rejection> {
        parts
            .extensions
            .get::<Claims>()
            .cloned()
            .ok_or(AppError::InvalidToken)
    }
}