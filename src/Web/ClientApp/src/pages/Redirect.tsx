import * as React from 'react';
import {CardContent, Typography} from "@mui/material";
import Layout from "../layouts/Layout";
import {useQuery} from "../hooks/router";
import {Helmet} from "react-helmet";
const Redirect = () => {

    let query = useQuery();
    const redirectUrl = query.get("redirectUrl");
    const redirect = () => {
        window.location.href = document.querySelector("meta[http-equiv=refresh]")!.getAttribute("data-url")!;
    }

    React.useEffect(() => {
        redirect();
    }, []);
    
    return (
        <Layout>
            <Helmet>
                <meta httpEquiv="refresh" content={`0;url=${redirectUrl}`} data-url={redirectUrl} />
            </Helmet>
            <CardContent>
                <Typography variant={"h4"}>You are now being returned to the application</Typography>
                <Typography variant={"body1"}>Once complete, you may close this tab.</Typography>
            </CardContent>
        </Layout>
    );
}

export default Redirect;
