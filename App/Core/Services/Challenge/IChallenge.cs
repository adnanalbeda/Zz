namespace Zz.App.Core;

public interface IChallenge
{
    public string RequestNewChallenge();

    public string ValidateChallengeAnswer(string challengeAnswerData);

    public bool ValidateChallengeTokenData(string challengeTokenData);
}
