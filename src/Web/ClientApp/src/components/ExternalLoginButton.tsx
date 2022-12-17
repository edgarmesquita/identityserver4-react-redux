import * as React from 'react';
import {
    Link as RouterLink,
} from 'react-router-dom';
import {Button} from "@mui/material";

interface IExternalLoginButtonProps {
    scheme: string;
    displayName: string;
    returnUrl?: string;
}

const schemes = [
    {name: "Google", color: "", icon: null}
]
const ExternalLoginButton = ({scheme, displayName, returnUrl}: IExternalLoginButtonProps) => {
    const schemeInfo = schemes.find(o => o.name === scheme);
    return (
        <Button component={RouterLink}
                to={`external/challenge?scheme=${scheme}&returnUrl=${returnUrl}`}
                type="submit"
                variant="contained"
                size="large"
                fullWidth sx={{mt: 2}}>
            {displayName}
        </Button>
    );
}

export default ExternalLoginButton;