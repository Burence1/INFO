package com.Info.InfoApp.auth.registration;

import org.springframework.stereotype.Service;

import java.util.function.Predicate;

@Service
public class emailValidator implements Predicate<String> {

    @Override
    public boolean test(String s){

        //TODO: EmailValidator regex
        return true;
    }
}
