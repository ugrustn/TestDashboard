import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import * as api from '../services/api';

interface AuthState {
    user: {
        id: string;
        email: string;
        firstName: string;
        lastName: string;
    } | null;
    token: string | null;
    loading: boolean;
    error: string | null;
}

const initialState: AuthState = {
    user: null,
    token: localStorage.getItem('token'),
    loading: false,
    error: null
};

export const login = createAsyncThunk(
    'auth/login',
    async ({ email, password }: { email: string; password: string }) => {
        const response = await api.login(email, password);
        const { token } = response.data;
        localStorage.setItem('token', token);
        return response.data;
    }
);

export const register = createAsyncThunk(
    'auth/register',
    async ({ email, password, firstName, lastName }: {
        email: string;
        password: string;
        firstName: string;
        lastName: string;
    }) => {
        const response = await api.register(email, password, firstName, lastName);
        const { token } = response.data;
        localStorage.setItem('token', token);
        return response.data;
    }
);

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout: (state) => {
            state.user = null;
            state.token = null;
            localStorage.removeItem('token');
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(login.fulfilled, (state, action) => {
                state.loading = false;
                state.token = action.payload.token;
                state.user = {
                    id: action.payload.id,
                    email: action.payload.email,
                    firstName: action.payload.firstName,
                    lastName: action.payload.lastName
                };
            })
            .addCase(login.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Login failed';
            })
            .addCase(register.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(register.fulfilled, (state, action) => {
                state.loading = false;
                state.token = action.payload.token;
                state.user = {
                    id: action.payload.id,
                    email: action.payload.email,
                    firstName: action.payload.firstName,
                    lastName: action.payload.lastName
                };
            })
            .addCase(register.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Registration failed';
            });
    }
});

export const { logout } = authSlice.actions;
export default authSlice.reducer; 