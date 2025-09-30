
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIJournal.Services
{
    public class OpenAIService
    {
        private readonly ChatClient _chatClient;

        public OpenAIService(string apiKey)
        {
            var openAIClient = new OpenAIClient(apiKey);
            _chatClient = openAIClient.GetChatClient("gpt-4o-mini"); // Using a valid, cost-effective model
        }

        public async Task<string> GetInsights(string prompt, string context = "")
        {
            try
            {
                var messages = new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage("You are an AI assistant that provides insights and analysis for journal entries. Your responses should be empathetic, insightful, and helpful. Provide thoughtful analysis while being supportive and encouraging.")
                };

                if (!string.IsNullOrEmpty(context))
                {
                    messages.Add(ChatMessage.CreateUserMessage($"Here's some context from previous entries: {context}"));
                }

                messages.Add(ChatMessage.CreateUserMessage(prompt));

                var response = await _chatClient.CompleteChatAsync(messages);

                if (response?.Value?.Content?.Count > 0)
                {
                    return response.Value.Content[0].Text?.Trim() ?? "I'm sorry, I couldn't generate insights at this time.";
                }
                else
                {
                    return "I'm sorry, I couldn't generate insights at this time.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting insights: {ex.Message}");
                return "I'm sorry, I couldn't generate insights at this time due to an error.";
            }
        }

        public async Task<string> GeneratePrompts(string mood = "", string topic = "")
        {
            try
            {
                var promptRequest = "Generate 3-5 thoughtful journal prompts";
                
                if (!string.IsNullOrEmpty(mood))
                {
                    promptRequest += $" for someone feeling {mood}";
                }
                
                if (!string.IsNullOrEmpty(topic))
                {
                    promptRequest += $" related to {topic}";
                }

                promptRequest += ". Make them introspective and encouraging.";

                var messages = new List<ChatMessage>
                {
                    ChatMessage.CreateSystemMessage("You are a helpful assistant that generates thoughtful journal prompts to help people reflect on their experiences and emotions."),
                    ChatMessage.CreateUserMessage(promptRequest)
                };

                var response = await _chatClient.CompleteChatAsync(messages);

                if (response?.Value?.Content?.Count > 0)
                {
                    return response.Value.Content[0].Text?.Trim() ?? "Here are some general prompts: What made you smile today? What challenged you? What are you grateful for?";
                }
                else
                {
                    return "Here are some general prompts: What made you smile today? What challenged you? What are you grateful for?";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating prompts: {ex.Message}");
                return "Here are some general prompts: What made you smile today? What challenged you? What are you grateful for?";
            }
        }
    }
}
