import {createAsyncThunk, createSlice, PayloadAction} from '@reduxjs/toolkit';
import {ILoginResult, ILoginState} from "./types";
import {RootState} from "../index";

const initialState: ILoginState = {
    loading: false
}

type JSONResponse<TData> = {
    data?: TData
    errors?: Array<{message: string}>
}

export const getLogin = createAsyncThunk(
    `login/get`,
    async (returnUrl: string, { getState }) => {
        const state = getState() as RootState;

        const response = await window.fetch(`account/login?returnUrl=${returnUrl}`, {
            method: 'GET',
            headers: {
                'content-type': 'application/json;charset=UTF-8',
            }
        });
        const data : ILoginResult = await response.json();
        return data;
    }
);

export const loginSlice = createSlice({
    name: 'login',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(getLogin.pending, (state) => {
                state.loading = true;
            })
            .addCase(getLogin.fulfilled, (state, action) => {
                state.info = action.payload;
                console.log(action)
            })
    }
});

export const getInfoState = (state: RootState) : ILoginResult | null | undefined => state.login.info;
export default loginSlice.reducer;