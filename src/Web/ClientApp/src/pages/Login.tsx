import * as React from 'react';
import {CardContent, TextField, Typography} from "@mui/material";
import Layout from "../layouts/Layout";
import {useValidator} from "../hooks/forms";
import {Validator} from "fluentvalidation-ts";

interface IUserForm {
    email?: string;
    password?: string;
}
class UserValidator extends Validator<IUserForm> {
    constructor() {
        super();

        this.ruleFor('email')
            .notNull()
            .withMessage('The first name is required.')
            .notEmpty()
            .withMessage('The first name is required.');
    }
}
const Home = () => {
    const [form, setForm] = React.useState<IUserForm>({});
    const [validate, userFormErrors] = useValidator(new UserValidator());
    const handleChange = (field: keyof IUserForm) => (event: React.ChangeEvent<HTMLInputElement>) => {
        const newState = { ...form, [field]: event.target.value };
        setForm(newState);
    };
    return (
        <Layout>
            <CardContent>
                <Typography variant={"h4"}>IdentityServer 4 With React</Typography>

                <TextField
                    id="email"
                    label="E-mail"
                    value={form?.email || ''}
                    onChange={handleChange("email")}
                    helperText={'email' in userFormErrors ? userFormErrors.email : ''}
                    variant="outlined" margin="normal" error={'email' in userFormErrors} fullWidth required
                />
                <TextField
                    id="password"
                    label="Password"
                    value={form?.password || ''}
                    onChange={handleChange("password")}
                    helperText={'password' in userFormErrors ? userFormErrors.password : ''}
                    variant="outlined" margin="normal" type="password" error={'password' in userFormErrors} fullWidth
                    required
                />
            </CardContent>
        </Layout>
    );
}

export default Home;
