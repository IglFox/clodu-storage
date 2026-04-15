use axum::{Json, extract::State};
use argon2::{Argon2, PasswordVerifier, PasswordHash};
use jsonwebtoken::{encode, EncodingKey, Header};

use crate::{db::DbPool, models::{LoginRequest, TokenResponse, Claims}, error::AppError, db::find_user_by_email};

const JWT_SECRET: &[u8] = b"change_this_to_env_variable_in_production";

pub async fn login(
    State(pool): State<DbPool>,
    Json(req): Json<LoginRequest>,
) -> Result<Json<TokenResponse>, AppError> {
    // Ищем пользователя
    let (user, password_hash) = find_user_by_email(&pool, &req.email)?
        .ok_or(AppError::InvalidCredentials)?;
    
    // Проверяем пароль
    let parsed_hash = PasswordHash::new(&password_hash)
        .map_err(|_| AppError::InvalidCredentials)?;
    
    Argon2::default()
        .verify_password(req.password.as_bytes(), &parsed_hash)
        .map_err(|_| AppError::InvalidCredentials)?;
    
    // Генерируем JWT
    let claims = Claims::new(user.id, user.email);
    let token = encode(
        &Header::default(),
        &claims,
        &EncodingKey::from_secret(JWT_SECRET),
    ).map_err(|_| AppError::InvalidCredentials)?;
    
    Ok(Json(TokenResponse::new(token)))
}