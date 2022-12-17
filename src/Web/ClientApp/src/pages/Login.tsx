import * as React from 'react';
import {
    Box,
    Button,
    CardContent,
    Checkbox,
    CircularProgress,
    FormControlLabel,
    TextField,
    Typography
} from "@mui/material";
import Layout from "../layouts/Layout";
import {useValidator} from "../hooks/forms";
import {Validator} from "fluentvalidation-ts";
import {useAppDispatch, useAppSelector} from "../hooks/store";
import {getInfoState, getLogin} from "../store/login/slice";
import {
    Link as RouterLink,
} from 'react-router-dom';
interface IUserForm {
    username?: string;
    password?: string;
    rememberLogin?: boolean;
    returnUrl?: string;
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
    const [form, setForm] = React.useState<IUserForm>({});
    const [validate, userFormErrors] = useValidator(new UserValidator());
    const loginInfo = useAppSelector(getInfoState);
    const dispatch = useAppDispatch();

    const handleChange = (field: keyof IUserForm) => (event: React.ChangeEvent<HTMLInputElement>) => {
        const newState = {...form, [field]: event.target.value};
        setForm(newState);
    };

    const loadInfo = () => {
        dispatch(getLogin("/"));
    }
    
    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        if (validate(form))
            event.preventDefault()
    }

    React.useEffect(() => {
        loadInfo();
    }, []);

    
    return (
        <Layout>
            <CardContent>
                <Typography variant={"h4"}>IdentityServer 4 With React</Typography>

                {!loginInfo && (
                    <Box sx={{ display: 'flex' }}>
                        <CircularProgress />
                    </Box>
                )}
                {loginInfo?.enableLocalLogin && (
                    <form method={"POST"} action={"/account/login"} onSubmit={handleSubmit}>
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
                                              control={<Checkbox/>}
                                              label={<Typography variant="body2">Remember me.</Typography>}
                                              labelPlacement="end"
                            />
                        )}

                        <Button type="submit" 
                                variant="contained" 
                                size="large" 
                                fullWidth sx={{mt: 2}}>
                            Sign In
                        </Button>
                    </form>
                )}
                {loginInfo?.visibleExternalProviders != null && loginInfo.visibleExternalProviders.length > 0 && (
                    <>
                        {loginInfo.visibleExternalProviders.map(o => (
                            <Button component={RouterLink} 
                                    to={`external/challenge?scheme=${o.authenticationScheme}&returnUrl=${loginInfo.returnUrl}`}
                                    type="submit" 
                                    variant="contained" 
                                    size="large" 
                                    fullWidth sx={{mt: 2}}>
                                {o.displayName}
                            </Button>
                        ))}
                    </>
                )}
            </CardContent>
        </Layout>
    );
}

export default Home;
