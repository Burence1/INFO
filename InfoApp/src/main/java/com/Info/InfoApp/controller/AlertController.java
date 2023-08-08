package com.Info.InfoApp.controller;

import com.Info.InfoApp.Entity.Alert;
import com.Info.InfoApp.model.AlertRequest;
import com.Info.InfoApp.service.AlertService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import java.util.Formatter;

@RestController
@Slf4j
public class AlertController {

    @Autowired
    private AlertService alertService;

    @PostMapping("/createAlert")
    private String createAlert(@RequestBody AlertRequest alertRequest){
        try{
            AlertRequest alert = alertService.saveNewAlert(alertRequest);

            return new Formatter().format("Successfully Created %s", alert.getAlertName()).toString();
        }
        catch (Exception e){
            return e.getMessage();
        }
    }
}
