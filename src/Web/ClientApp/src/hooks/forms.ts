import * as React from 'react';
import {nanoid} from '@reduxjs/toolkit';
import {Validator} from 'fluentvalidation-ts';
import type {ChangeEvent} from 'react';

export type FormErrors<TEntity> = {
    [key in keyof TEntity]?: string;
};

export const useForm =
    <TContent>(defaultValues: TContent) =>
        (handler: (content: TContent) => void) =>
            async (event: ChangeEvent<HTMLFormElement>) => {
                event.preventDefault();
                event.persist();

                const form = event.target as HTMLFormElement;
                const elements = Array.from(form.elements) as HTMLInputElement[];
                const data = elements
                    .filter((element) => element.hasAttribute('name'))
                    .reduce(
                        (object, element) => ({
                            ...object,
                            [`${element.getAttribute('name')}`]: element.value
                        }),
                        defaultValues
                    );
                await handler(data);
                form.reset();
            };

export function useValidator<TEntity>(validator: Validator<TEntity>) {
    const [errors, setErrors] = React.useState<FormErrors<TEntity>>({});
    const [isValid, setIsValid] = React.useState<boolean>(true);
    const [key, setKey] = React.useState<string>(nanoid());

    const validate = (data: Partial<TEntity>) => {
        setErrors({});
        setKey(nanoid());
        const result = validator.validate(data as TEntity) as any;
        if (Object.keys(result).length > 0) {
            setErrors({...result});
            setIsValid(false);
            return false;
        }
        setIsValid(true);
        return true;
    };
    
    return [validate, errors, isValid, key] as const;
}