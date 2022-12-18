import * as React from 'react';
import {
    Alert, AlertTitle,
    Box,
    Button,
    CardContent,
    Checkbox,
    CircularProgress,
    FormControlLabel, Link, Snackbar, Stack,
    TextField,
    Typography
} from "@mui/material";
import Layout from "../layouts/Layout";
import {useValidator} from "../hooks/forms";
import {Validator} from "fluentvalidation-ts";
import {useAppDispatch, useAppSelector} from "../hooks/store";
import {getInfoState, getLogin} from "../store/login/slice";
import {
    redirect,
    Link as RouterLink
} from 'react-router-dom';
import {useQuery} from "../hooks/router";
import ExternalLoginButton from "../components/ExternalLoginButton";

interface IUserForm {
    username?: string;
    password?: string;
    rememberLogin?: boolean;
    returnUrl?: string;
}

interface IRedirectResponse {
    returnUrl: string;
}

class UserValidator extends Validator<IUserForm> {
    constructor() {
        super();

        this.ruleFor('username')
            .notNull()
            .withMessage('The e-mail is required.')
            .notEmpty()
            .withMessage('The e-mail is required.');

        this.ruleFor('password')
            .notNull()
            .withMessage('The password is required.')
            .notEmpty()
            .withMessage('The password is required.');
    }
}

const Home = () => {
    const loginInfo = useAppSelector(getInfoState);
    const [form, setForm] = React.useState<IUserForm>({
        rememberLogin: loginInfo?.rememberLogin,
        returnUrl: loginInfo?.returnUrl
    });
    const [validate, userFormErrors] = useValidator(new UserValidator());
    const [open, setOpen] = React.useState(false);
    const dispatch = useAppDispatch();
    let query = useQuery();

    const handleClick = () => {
        setOpen(true);
    };

    const handleClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
        if (reason === 'clickaway') {
            return;
        }

        setOpen(false);
    };

    const handleChange = (field: keyof IUserForm) => (event: React.ChangeEvent<HTMLInputElement>) => {
        const newState = {...form, [field]: event.target.value};
        setForm(newState);
    };

    const handleCheckboxChange = (field: keyof IUserForm) => (event: React.ChangeEvent<HTMLInputElement>) => {
        const newState = {...form, [field]: event.target.checked};
        setForm(newState);
    };

    const loadInfo = () => {
        dispatch(getLogin(query.get("returnUrl") || "/"));
    }

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!validate(form))
            return;

        console.log(event)
        const response = await window.fetch('/account/login', {
            method: 'POST',
            headers: {
                'content-type': 'application/json;charset=UTF-8',
            },
            body: JSON.stringify(form)
        });

        if (!response.ok) {
            setOpen(true);
            return;
        }
        const data: IRedirectResponse = await response.json();
        redirect(data.returnUrl);
    }

    React.useEffect(() => {
        loadInfo();
    }, []);

    return (
        <Layout>
            <CardContent>
                <Typography variant={"h4"} gutterBottom>Login</Typography>

                {!loginInfo && (
                    <Box sx={{display: 'flex'}}>
                        <CircularProgress/>
                    </Box>
                )}
                {loginInfo?.enableLocalLogin && (
                    <form method={"POST"} action={"/account/login"} onSubmit={handleSubmit}>
                        <input type="hidden" name="returnUrl" value={form.returnUrl}/>
                        <TextField
                            name="username"
                            label="E-mail"
                            value={form?.username || ''}
                            onChange={handleChange("username")}
                            helperText={'username' in userFormErrors ? userFormErrors.username : ''}
                            variant="outlined"
                            margin="normal"
                            error={'username' in userFormErrors}
                            fullWidth
                            required
                        />
                        <TextField
                            name="password"
                            label="Password"
                            value={form?.password || ''}
                            onChange={handleChange("password")}
                            helperText={'password' in userFormErrors ? userFormErrors.password : ''}
                            variant="outlined"
                            margin="normal"
                            type="password"
                            error={'password' in userFormErrors}
                            fullWidth
                            required
                        />
                        {loginInfo?.allowRememberLogin && (
                            <FormControlLabel name="rememberLogin"
                                              sx={{my: 1}}
                                              value="true"
                                              control={<Checkbox checked={form.rememberLogin || false}
                                                                 onChange={handleCheckboxChange("rememberLogin")}/>}
                                              label={<Typography variant="body2">Remember me.</Typography>}
                                              labelPlacement="end"
                            />
                        )}
                        <Link component={RouterLink} to="/forgot-password">
                            Forgot password?
                        </Link>
                        <Button type="submit"
                                variant="contained"
                                size="large"
                                fullWidth sx={{mt: 2}}>
                            Sign In
                        </Button>
                        
                        <Box sx={{
                            display: 'flex', 
                            alignItems: 'center',
                            margin: '16px 0',
                            '&::before': {
                                backgroundColor: 'rgba(0,0,0,0.35)',
                                content: "''",
                                height: '1px',
                                width: '50%'
                            },
                            '&::after': {
                                backgroundColor: 'rgba(0,0,0,0.35)',
                                content: "''",
                                height: '1px',
                                width: '50%'
                            }
                        }}>
                            <Typography variant="caption" sx={{padding: '0 12px'}}>or</Typography>
                        </Box>
                    </form>
                )}
                {loginInfo?.visibleExternalProviders != null && loginInfo.visibleExternalProviders.length > 0 && (
                    <>
                        {loginInfo.visibleExternalProviders.map(o => (
                            <ExternalLoginButton
                                key={o.authenticationScheme}
                                scheme={o.authenticationScheme}
                                displayName={o.displayName}
                                returnUrl={loginInfo?.returnUrl}/>
                        ))}
                    </>
                )}
                {loginInfo && !loginInfo.enableLocalLogin && loginInfo.visibleExternalProviders.length === 0 && (
                    <Alert severity="warning">
                        <AlertTitle>Invalid login request</AlertTitle>
                        There are no login schemes configured for this request.
                    </Alert>
                )}
            </CardContent>

            <Snackbar open={open}
                      anchorOrigin={{vertical: 'top', horizontal: 'center'}}
                      autoHideDuration={10000} onClose={handleClose}>
                <Alert onClose={handleClose} severity="error" sx={{width: '100%'}}>
                    One error has occurred!
                </Alert>
            </Snackbar>

        </Layout>
    );
}

export default Home;
