package com.Info.InfoApp.service;

import com.Info.InfoApp.Entity.Alert;
import com.Info.InfoApp.auth.config.UserInfoDetails;
import com.Info.InfoApp.model.AlertRequest;
import com.Info.InfoApp.repository.AlertRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class AlertServiceImpl implements AlertService{
//    @Autowired
//    private UserInfoDetails userInfoDetails;
    @Autowired
    private AlertRepository alertRepository;

//    String loggedUser = userInfoDetails.getUsername();
    @Override
    public AlertRequest saveNewAlert(AlertRequest alertRequest) {
        Alert alert = new Alert();
        alert.setAlertNo(alertRequest.getAlertNo());
        alert.setAlertName(alertRequest.getAlertName());
        alert.setAlertSubject(alertRequest.getAlertSubject());
        alert.setEnabled(true);
        alert.setMessageBody(alertRequest.getMessageBody());
        alert.setCaptureUser("loggedUser");
        alertRepository.save(alert);

        return alertRequest;
    }
}
